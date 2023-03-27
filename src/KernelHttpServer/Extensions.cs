﻿// Copyright (c) Microsoft. All rights reserved.

using KernelHttpServer.Config;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.CoreSkills;
using Microsoft.SemanticKernel.KernelExtensions;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using Microsoft.SemanticKernel.Skills.Document;
using Microsoft.SemanticKernel.Skills.Document.FileSystem;
using Microsoft.SemanticKernel.Skills.Document.OpenXml;
using Microsoft.SemanticKernel.Skills.Web;
using Microsoft.SemanticKernel.TemplateEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static KernelHttpServer.Config.Constants;
using Directory = System.IO.Directory;

namespace KernelHttpServer;

internal static class Extensions
{
    internal static ApiKeyConfig ToApiKeyConfig(this HttpRequestData req)
    {
        var apiConfig = new ApiKeyConfig();

        // completion values
        if (req.Headers.TryGetValues(SKHttpHeaders.CompletionBackend, out var completionAIService))
        {
            apiConfig.CompletionConfig.AIService = Enum.Parse<AIService>(completionAIService.First());
        }

        if (req.Headers.TryGetValues(SKHttpHeaders.CompletionModel, out var completionModelValue))
        {
            apiConfig.CompletionConfig.DeploymentOrModelId = completionModelValue.First();
            apiConfig.CompletionConfig.Label = apiConfig.CompletionConfig.DeploymentOrModelId;
        }

        if (req.Headers.TryGetValues(SKHttpHeaders.CompletionEndpoint, out var completionEndpoint))
        {
            apiConfig.CompletionConfig.Endpoint = completionEndpoint.First();
        }

        if (req.Headers.TryGetValues(SKHttpHeaders.CompletionKey, out var completionKey))
        {
            apiConfig.CompletionConfig.Key = completionKey.First();
        }

        // embedding values
        if (req.Headers.TryGetValues(SKHttpHeaders.EmbeddingBackend, out var embeddingAIService))
        {
            apiConfig.EmbeddingConfig.AIService = Enum.Parse<AIService>(embeddingAIService.First());
        }

        if (req.Headers.TryGetValues(SKHttpHeaders.EmbeddingModel, out var embeddingModelValue))
        {
            apiConfig.EmbeddingConfig.DeploymentOrModelId = embeddingModelValue.First();
            apiConfig.EmbeddingConfig.Label = apiConfig.EmbeddingConfig.DeploymentOrModelId;
        }

        if (req.Headers.TryGetValues(SKHttpHeaders.EmbeddingEndpoint, out var embeddingEndpoint))
        {
            apiConfig.EmbeddingConfig.Endpoint = embeddingEndpoint.First();
        }

        if (req.Headers.TryGetValues(SKHttpHeaders.EmbeddingKey, out var embeddingKey))
        {
            apiConfig.EmbeddingConfig.Key = embeddingKey.First();
        }

        return apiConfig;
    }

    internal static async Task<HttpResponseData> CreateResponseWithMessageAsync(this HttpRequestData req, HttpStatusCode statusCode, string message)
    {
        HttpResponseData response = req.CreateResponse(statusCode);
        await response.WriteStringAsync(message);
        return response;
    }

    internal static ISKFunction GetFunction(this IReadOnlySkillCollection skills, string skillName, string functionName)
    {
        return skills.HasNativeFunction(skillName, functionName)
            ? skills.GetNativeFunction(skillName, functionName)
            : skills.GetSemanticFunction(skillName, functionName);
    }

    internal static bool HasSemanticOrNativeFunction(this IReadOnlySkillCollection skills, string skillName, string functionName)
    {
        return skills.HasSemanticFunction(skillName, functionName) || skills.HasNativeFunction(skillName, functionName);
    }  

    internal static void RegisterPlanner(this IKernel kernel)
    {
        PlannerSkill planner = new(kernel);
        _ = kernel.ImportSkill(planner, nameof(PlannerSkill));
    }

    internal static void RegisterTextMemory(this IKernel kernel)
    {
        _ = kernel.ImportSkill(new TextMemorySkill(), nameof(TextMemorySkill));
    }

    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope",
        Justification = "The caller invokes native skills during a request and the skill instances must remain alive for those requests to be successful.")]
    internal static void RegisterNativeSkills(this IKernel kernel, IEnumerable<string>? skillsToLoad = null)
    {
        if (ShouldLoad(nameof(DocumentSkill), skillsToLoad))
        {
            DocumentSkill documentSkill = new(new WordDocumentConnector(), new LocalFileSystemConnector());
            _ = kernel.ImportSkill(documentSkill, nameof(DocumentSkill));
        }

        if (ShouldLoad(nameof(ConversationSummarySkill), skillsToLoad))
        {
            ConversationSummarySkill conversationSummarySkill = new(kernel);
            _ = kernel.ImportSkill(conversationSummarySkill, nameof(ConversationSummarySkill));
        }

        if (ShouldLoad(nameof(WebFileDownloadSkill), skillsToLoad))
        {
            var webFileDownloadSkill = new WebFileDownloadSkill();
            _ = kernel.ImportSkill(webFileDownloadSkill, nameof(WebFileDownloadSkill));
        }

    }

    internal static void RegisterSemanticSkills(
        this IKernel kernel,
        string skillsFolder,
        ILogger logger,
        IEnumerable<string>? skillsToLoad = null)
    {
        foreach (string skPromptPath in Directory.EnumerateFiles(skillsFolder, "*.txt", SearchOption.AllDirectories))
        {
            FileInfo fInfo = new(skPromptPath);
            DirectoryInfo? currentFolder = fInfo.Directory;

            while (currentFolder?.Parent?.FullName != skillsFolder)
            {
                currentFolder = currentFolder?.Parent;
            }

            if (ShouldLoad(currentFolder.Name, skillsToLoad))
            {
                try
                {
                    _ = kernel.ImportSemanticSkillFromDirectory(skillsFolder, currentFolder.Name);
                }
                catch (TemplateException e)
                {
                    logger.LogWarning("Could not load skill from {0} with error: {1}", currentFolder.Name, e.Message);
                }
            }
        }
    }

    private static bool ShouldLoad(string skillName, IEnumerable<string>? skillsToLoad = null)
    {
        return skillsToLoad?.Contains(skillName, StringComparer.InvariantCultureIgnoreCase) != false;
    }
}

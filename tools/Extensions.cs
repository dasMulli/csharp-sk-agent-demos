public static async Task<Azure.AI.Projects.Agent> CreateOrUpdateAgentAsync(this Azure.AI.Projects.AgentsClient agentClient,
    string model, string name = null, string description = null, string instructions = null, IEnumerable<ToolDefinition> tools = null, ToolResources toolResources = null, float? temperature = null, float? topP = null, BinaryData responseFormat = null, IReadOnlyDictionary<string, string> metadata = null, CancellationToken cancellationToken = default)
{
    Azure.AI.Projects.Agent existingAgent = null;
    try
    {
        foreach (var agent in (await agentClient.GetAgentsAsync(cancellationToken: cancellationToken)).Value)
        {
            if (agent.Name == name)
            {
                existingAgent = agent;
                break;
            }
        }
    }
    catch (Azure.RequestFailedException ex) when (ex.Status == 404)
    {
        // Agent not found, proceed to create a new one
    }

    if (existingAgent != null)
    {
        Console.WriteLine($"Agent with name '{name}' already exists. Updating the existing agent.");
        // Update the existing agent
        return await agentClient.UpdateAgentAsync(existingAgent.Id, model, name, description, instructions, tools, toolResources, temperature, topP, responseFormat, metadata, cancellationToken);
    }
    else
    {
        Console.WriteLine($"Creating a new agent with name '{name}'.");
        return await agentClient.CreateAgentAsync(model, name, description, instructions, tools, toolResources, temperature, topP, responseFormat, metadata, cancellationToken);
    }
}
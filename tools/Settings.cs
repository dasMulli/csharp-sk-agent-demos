public static class Settings
{
    public static string AIFoundryProjectConnectionString { get; set; } = "swedencentral.api.azureml.ms;ceb785cd-ccc1-4604-809f-dcadbea7f433;rg-moneta-agent-demo-aitour;proj-moneta-agent-demo-aitour";
    public static string ModelDeploymentName { get; set; } = "gpt-4o-2024-11-20";
    public static string EmbeddingModelDeploymentName { get; set; } = "text-embedding-3-large";
    public static string CosmosDbUri { get; set; } = "https://cdbb7rkl2ft6pa2o.documents.azure.com:443/";
}
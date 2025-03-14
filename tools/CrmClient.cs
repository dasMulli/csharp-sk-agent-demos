
class CrmClient
{
    private CosmosClient cosmosClient = new CosmosClient(Settings.CosmosDbUri, new DefaultAzureCredential(), new CosmosClientOptions()
    {
        UseSystemTextJsonSerializerWithOptions = new JsonSerializerOptions() { WriteIndented = true }
    });


    [KernelFunction("load_from_crm_by_client_fullname")]
    [Description("Load insured client data from the CRM from the given full name")]
    public async Task<string> GetCustomerByFullName(string fullName)
    {
        var container = cosmosClient.GetContainer("rminsights", "clientdata");
        var query = $"SELECT * FROM c WHERE c.fullName LIKE @fullName";
        var iterator = container.GetItemQueryIterator<JsonDocument>(new QueryDefinition(query).WithParameter("@fullName", fullName));
        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault().RootElement.GetRawText();
        }
        return null;
    }

    [KernelFunction("load_from_crm_by_client_id")]
    [Description("Load insured client data from the CRM from the client_id")]
    public async Task<string> GetCustomerById(string clientId)
    {
        var container = cosmosClient.GetContainer("rminsights", "clientdata");
        var query = $"SELECT * FROM c WHERE c.clientID = @clientId";
        var iterator = container.GetItemQueryIterator<JsonDocument>(new QueryDefinition(query).WithParameter("@clientId", clientId));
        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.FirstOrDefault().RootElement.GetRawText();
        }
        return null;
    }
}
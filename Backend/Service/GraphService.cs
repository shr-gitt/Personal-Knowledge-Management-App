using Backend.DTO;
using Neo4j.Driver;

namespace Backend.Service;

public class GraphService
{
    private readonly Neo4jService _neo4jService;
    private readonly ILogger<GraphService> _logger;
    
    public GraphService(Neo4jService neo4JService,ILogger<GraphService> logger)
    {
        _neo4jService = neo4JService;
        _logger = logger;
    }

    public async Task<ServiceResponse<object>> GetUserGraph(string username)
    {
        try
        {
            string cypher = @"
                MATCH (u:User {user_id: $username})-[:WRITES]->(n:Note)
                OPTIONAL MATCH (n)-[:HAS_TAG]->(t:Tag)
                RETURN n, t
            ";

            var records = await _neo4jService.RunQuery(cypher, new { username });
            
            _logger.LogInformation($"Graph returned {records.Count} records");

            var nodes = new HashSet<object>();
            var links = new HashSet<object>();

            foreach (var record in records)
            {
                var note = record["n"] as INode;
                var tag = record["t"] as INode;
                
                if (note != null)
                {
                    nodes.Add(new
                    {
                        id = note.ElementId.ToString(),
                        label = note.Properties["title"].ToString(),
                        type = "note"
                    });
                }

                if (tag != null && note != null)
                {
                    nodes.Add(new
                    {
                        id = tag.ElementId.ToString(),
                        label = tag.Properties["tag_name"].ToString(),
                        type = "tag"
                    });

                    links.Add(new
                    {
                        source = note.Id.ToString(),
                        target = tag.Id.ToString()
                    });
                }

            }

            return new ServiceResponse<object>
                { Success = true, Message = "User graph received", Data = new { nodes, links } };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching graph");
            
            return new ServiceResponse<object>{Success = false, Message = ex.Message};
        }
    }
}
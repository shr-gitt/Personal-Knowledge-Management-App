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
                MATCH (u:User {username: $username})-[:OWNS]->(n:Note)
                OPTIONAL MATCH (n)-[:TAGGED_WITH]->(t:Tag)
                OPTIONAL MATCH (n)-[:LINKS_TO]->(m:Note)
                RETURN n, t, m
            ";

            var records = await _neo4jService.RunQuery(cypher, new { username });

            var nodes = new HashSet<object>();
            var links = new HashSet<object>();

            foreach (var record in records)
            {
                var note = record["n"] as INode;
                var tag = record["t"] as INode;
                var linkedNote = record["m"] as INode;

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
                        label = tag.Properties["name"].ToString(),
                        type = "tag"
                    });

                    links.Add(new
                    {
                        source = note.Id.ToString(),
                        target = tag.Id.ToString()
                    });
                }

                if (linkedNote != null && note != null)
                {
                    nodes.Add(new
                    {
                        id = linkedNote.Id.ToString(),
                        label = linkedNote.Properties["title"].ToString(),
                        type = "note"
                    });

                    links.Add(new
                    {
                        source = note.Id.ToString(),
                        target = linkedNote.Id.ToString()
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
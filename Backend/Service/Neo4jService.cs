using Neo4j.Driver;

namespace Backend.Service;

public class Neo4jService
{
    private readonly IDriver _driver;
    private readonly ILogger<Neo4jService> _logger;

    public Neo4jService(string Uri, string UserName, string Password, ILogger<Neo4jService> logger)
    {
        _driver = GraphDatabase.Driver(Uri, AuthTokens.Basic(UserName, Password));
        _logger = logger;
    }

    public async Task CreateUserNodeAsync(string userId, string userName)
    {
        var session = _driver.AsyncSession();

        try
        {
            var result = await session.RunAsync(
                "MERGE (u:User {user_id: $userId, user_name: $userName})",
                new { userId, userName });
            _logger.LogInformation("User {user_id} node created or resused", userId);
        }
        catch (Neo4jException neoEx)
        {
            _logger.LogError(neoEx, "Neo4j error occurred while creating or reusing User '{userName}'", userName);
            throw new Exception($"Error interacting with Neo4j database while handling user '{userName}'", neoEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while creating or reusing user '{userName}'", userName);
            throw new Exception($"Unexpected error occurred while handling user '{userName}'", ex);
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task CreateNoteNodeAsync(string noteId, string title, string content, string userId)
    {
        var session = _driver.AsyncSession();

        try
        {
            var result = await session.RunAsync(
                "MERGE (n:Note {note_id: $noteId, title: $title, content: $content})-[:WRITES]->(u:User{id:$userId})",
                new { noteId, title, content, userId });
            _logger.LogInformation("Note {note_id} node created or reused", noteId);
        }
        catch (Neo4jException neoEx)
        {
            _logger.LogError(neoEx, "Neo4j error occurred while creating or reusing note '{noteId}'", noteId);
            throw new Exception($"Error interacting with Neo4j database while handling note '{noteId}'", neoEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while creating or reusing note '{noteId}'", noteId);
            throw new Exception($"Unexpected error occurred while handling note '{noteId}'", ex);
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task CreateTagNodeAsync(string tagName)
    {
        var session = _driver.AsyncSession();

        try
        {
            var result = await session.RunAsync(
                "MERGE (tag:Tag {tag_name: $tagName})",
                new { tagName });
            _logger.LogInformation("Tag {tag_name} node created", tagName);
        }
        catch (Neo4jException neoEx)
        {
            _logger.LogError(neoEx, "Neo4j error occurred while creating or reusing tag '{tag}'", tagName);
            throw new Exception($"Error interacting with Neo4j database while handling tag '{tagName}'", neoEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while creating or reusing tag '{tag}'", tagName);
            throw new Exception($"Unexpected error occurred while handling tag '{tagName}'", ex);
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task CreateRelationshipNodeAsync(string noteId, string tagName)
    {
        var session = _driver.AsyncSession();

        try
        {
            await session.RunAsync(
                "MATCH (n:Note {id: $noteId}),(t:Tag {tag_name: $tagName})" +
                "MERGE (n)-[:HAS_TAG]->(t)",
                new { noteId, tagName });
            _logger.LogInformation("Note {note_id} node and Tag {tagName} node relationship created", noteId, tagName);
        }
        catch (Neo4jException neoEx)
        {
            _logger.LogError(neoEx, "Neo4j error occurred while creating relationship '{noteId}' and '{tag}'",noteId, tagName);
            throw new Exception($"Error interacting with Neo4j database while handling note '{noteId}' and tag '{tagName}'", neoEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while creating relationship note '{noteId}' tag '{tag}'",noteId, tagName);
            throw new Exception($"Unexpected error occurred while handling relationship note '{noteId}' and tag '{tagName}'", ex);
        }
        finally
        {
            await session.CloseAsync();
        }
    }

    public async Task DeleteNodeAsync(string nodeId)
    {
        var session = _driver.AsyncSession();

        try
        {
            await session.RunAsync(
                "MATCH (n:Note {id: $noteId}) DETACH DELETE n",
                new { nodeId });
            _logger.LogInformation(" {node_id} node deleted", nodeId);
        }
        catch (Neo4jException neoEx)
        {
            _logger.LogError(neoEx, "Neo4j error occurred while deleting node '{nodeId}'", nodeId);
            throw new Exception($"Error interacting with Neo4j database while handling tag '{nodeId}'", neoEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while deleting '{nodeId}'", nodeId);
            throw new Exception($"Unexpected error occurred while handling node '{nodeId}'", ex);
        }
        finally
        {
            await session.CloseAsync();
        }
    }
}
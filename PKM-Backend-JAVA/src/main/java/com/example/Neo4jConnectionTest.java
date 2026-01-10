package com.example;

import org.neo4j.driver.*;

public class Neo4jConnectionTest {

    public static void main(String[] args) {
        // Define the connection URL for your Neo4j instance
        String uri = "bolt://localhost:7687";
        String user = "neo4j";
        String password = "Neo4jSettings";

        // Create a driver instance
        try (Driver driver = GraphDatabase.driver(uri, AuthTokens.basic(user, password))) {
            // Create a session and run a simple query to test the connection
            try (Session session = driver.session()) {
                Result result = session.run("RETURN 'Connection Successful!' AS message");
                while (result.hasNext()) {
                    System.out.println(result.next().get("message").asString());
                }
            }
        } catch (Exception e) {
            // If the connection fails, print the error
            System.err.println("Error connecting to Neo4j: " + e.getMessage());
            e.printStackTrace();
        }
    }
}

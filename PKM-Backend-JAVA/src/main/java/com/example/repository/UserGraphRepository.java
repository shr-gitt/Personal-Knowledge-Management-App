package com.example.repository;

import com.example.model.UserNode;
import org.springframework.data.neo4j.core.schema.Node;
import org.springframework.data.neo4j.repository.Neo4jRepository;

public interface UserGraphRepository extends Neo4jRepository<UserNode, String> {

}

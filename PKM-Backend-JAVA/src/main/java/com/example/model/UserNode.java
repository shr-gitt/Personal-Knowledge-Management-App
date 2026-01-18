package com.example.model;

import lombok.Getter;
import lombok.Setter;
import org.springframework.data.neo4j.core.schema.GeneratedValue;
import org.springframework.data.neo4j.core.schema.Id;
import org.springframework.data.neo4j.core.schema.Node;

@Node("User")
@Getter
@Setter
public class UserNode {
    @Id
    private String id = java.util.UUID.randomUUID().toString();

    private String username;

    public UserNode(){}
    public UserNode(String username){
        this.username = username;
    }
}

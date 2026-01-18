package com.example.model;

import jakarta.validation.constraints.NotBlank;
import lombok.Getter;
import lombok.Setter;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import java.util.Date;
import java.util.List;

@Document(collection = "Notes")
@Getter
@Setter
public class Note {
    @Id
    private String id;
    @NotBlank
    private String userId;

    private String title;
    private String content;
    private List<String> tags;

    private Date created;
    private Date updated;
    private boolean isArchived;

    public Note() {}
    public Note(String userId, String title, String content, List<String> tags, Date created, Date updated, boolean isArchived) {
        this.userId = userId;
        this.title = title;
        this.content = content;
        this.tags = tags;
        this.created = created;
        this.updated = updated;
        this.isArchived = isArchived;
    }
}

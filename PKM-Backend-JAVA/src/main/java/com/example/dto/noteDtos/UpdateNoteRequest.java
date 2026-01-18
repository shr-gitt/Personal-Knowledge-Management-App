package com.example.dto.noteDtos;

import jakarta.validation.constraints.NotBlank;
import lombok.Getter;
import lombok.Setter;

import java.util.List;

@Getter
public class UpdateNoteRequest {
    @NotBlank
    private String noteId;

    @NotBlank
    private String noteTitle;
    private String noteContent;
    private List<String> tags;
}

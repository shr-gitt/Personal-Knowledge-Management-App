package com.example.dto.noteDtos;

import jakarta.validation.constraints.NotBlank;
import lombok.Getter;
import lombok.Setter;

@Getter
public class UpdateNoteRequest {
    @NotBlank
    private String noteId;

    @NotBlank
    private String noteTitle;
    private String noteContent;
}

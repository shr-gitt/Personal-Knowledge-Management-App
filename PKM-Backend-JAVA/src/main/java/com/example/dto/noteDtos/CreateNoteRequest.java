package com.example.dto.noteDtos;

import jakarta.validation.constraints.NotBlank;
import lombok.Getter;

@Getter
public class CreateNoteRequest {
    @NotBlank
    private String userId;

    @NotBlank
    private String title;
    private String content;
}

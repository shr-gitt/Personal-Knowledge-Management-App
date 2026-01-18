package com.example.controller;

import com.example.dto.noteDtos.CreateNoteRequest;
import com.example.response.ApiResponse;
import com.example.service.NotesService;
import jakarta.validation.Valid;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class NotesController {
    private NotesService notesService;

    public NotesController(NotesService notesService) {
        this.notesService = notesService;
    }

    @PostMapping(value = "/createNote")
    public ResponseEntity<ApiResponse<Object>> createNote(@Valid @RequestBody CreateNoteRequest request) {
        return ResponseEntity.ok(notesService.createNote(request));
    }
}

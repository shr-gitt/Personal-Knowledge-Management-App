package com.example.service;

import com.example.dto.noteDtos.CreateNoteRequest;
import com.example.exception.ValidationException;
import com.example.model.Note;
import com.example.repository.NoteRepository;
import com.example.response.ApiResponse;
import org.springframework.stereotype.Service;

import java.sql.Date;
import java.time.LocalDate;

import static com.example.response.ApiResponse.success;

@Service
public class NotesService {
    private NoteRepository noteRepository;

    public NotesService(NoteRepository noteRepository) {
        this.noteRepository = noteRepository;
    }

    public ApiResponse createNote(CreateNoteRequest createNoteRequest) {
        if(createNoteRequest.getUserId() == null) {
            throw new ValidationException("User id is required");
        }

        if(createNoteRequest.getTitle().isEmpty() || createNoteRequest.getTitle() == null) {
            throw new ValidationException("Title cannot be empty");
        }

        Note note = new Note();
        note.setTitle(createNoteRequest.getTitle());
        note.setContent(createNoteRequest.getContent());
        note.setUserId(createNoteRequest.getUserId());
        note.setCreated(Date.valueOf(LocalDate.now()));
        note.setUpdated(Date.valueOf(LocalDate.now()));
        note.setArchived(false);
        noteRepository.save(note);

        return success("Note successfully created", note);
    }
}

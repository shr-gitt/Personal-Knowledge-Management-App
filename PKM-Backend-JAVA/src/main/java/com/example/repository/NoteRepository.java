package com.example.repository;

import com.example.model.Note;
import org.springframework.data.mongodb.repository.MongoRepository;

import java.util.Optional;

public interface NoteRepository extends MongoRepository<Note,String> {
    Optional<Note> findByUserIdAndId(String userId, String id);
    Optional<Note> findById(String id);
}

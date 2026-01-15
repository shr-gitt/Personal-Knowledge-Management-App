package com.example.service;

import com.example.dto.SignUpRequest;
import com.example.exception.BusinessException;
import com.example.model.User;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import com.example.repository.UserRepository;
import com.example.response.ApiResponse;

import static com.example.response.ApiResponse.success;

@Service
public class AuthService {
    private UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    public AuthService(UserRepository userRepository,  PasswordEncoder passwordEncoder) {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
    }

    public ApiResponse register(SignUpRequest request) {
        if(userRepository.existsByEmail(request.getEmail()))
            throw new BusinessException("Email already exists");

        if (userRepository.existsByUsername(request.getUsername()))
            throw new BusinessException("Username already exists");

        User user = new User();

        user.setUsername(request.getUsername());
        user.setName(request.getName());
        user.setEmail(request.getEmail());
        user.setPassword(passwordEncoder.encode(request.getPassword()));
        user.setPhoneNumber(request.getPhoneNumber());
        userRepository.save(user);

        return success("User successfully registered", user);
    }
}

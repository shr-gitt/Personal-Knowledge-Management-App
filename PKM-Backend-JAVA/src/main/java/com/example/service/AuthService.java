package com.example.service;

import com.example.dto.SignInRequest;
import com.example.dto.SignUpRequest;
import com.example.exception.BusinessException;
import com.example.model.User;
import jakarta.transaction.Transactional;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import com.example.repository.UserRepository;
import com.example.response.ApiResponse;

import static com.example.response.ApiResponse.success;

@Service
public class AuthService {
    private UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final UploadImageService uploadImageService;
    private final CustomUserDetailsService customUserDetailsService;
    public AuthService(UserRepository userRepository,  PasswordEncoder passwordEncoder, UploadImageService uploadImageService, CustomUserDetailsService customUserDetailsService) {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
        this.uploadImageService = uploadImageService;
        this.customUserDetailsService = customUserDetailsService;
    }

    @Transactional
    public ApiResponse register(SignUpRequest request) throws Exception {
        if(userRepository.existsByEmail(request.getEmail()))
            throw new BusinessException("Email already exists");

        if (userRepository.existsByUsername(request.getUsername()))
            throw new BusinessException("Username already exists");

        String imagePath = null;

        if (request.getImageFile() != null) {
            imagePath = uploadImageService.uploadImage(request.getImageFile());
        }

        User user = new User();

        user.setUsername(request.getUsername());
        user.setName(request.getName());
        user.setEmail(request.getEmail());
        user.setPassword(passwordEncoder.encode(request.getPassword()));
        user.setPhoneNumber(request.getPhoneNumber());
        user.setImage(imagePath);
        userRepository.save(user);

        return success("User successfully registered", user);
    }

    public ApiResponse signIn(SignInRequest request) {
        UserDetails userDetails = customUserDetailsService.loadUserByUsername(request.getUsername());

        if(passwordEncoder.matches(request.getPassword(), userDetails.getPassword()))
            return success("User successfully signed in", userDetails);
        else
            throw new BusinessException("Incorrect password. Please try again");
    }
}

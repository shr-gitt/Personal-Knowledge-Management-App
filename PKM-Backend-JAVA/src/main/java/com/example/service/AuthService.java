package com.example.service;

import com.example.dto.SignInRequest;
import com.example.dto.SignUpRequest;
import com.example.exception.BusinessException;
import com.example.model.User;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import com.example.repository.UserRepository;
import com.example.response.ApiResponse;

import static com.example.response.ApiResponse.success;

@Service
public class AuthService {
    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final UploadImageService uploadImageService;
    private final AuthenticationManager authenticationManager;
    public AuthService(
            UserRepository userRepository,
            PasswordEncoder passwordEncoder,
            UploadImageService uploadImageService,
            AuthenticationManager authenticationManager) {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
        this.uploadImageService = uploadImageService;
        this.authenticationManager = authenticationManager;
    }

    //@Transactional(transactionManager = "transactionManager")
    public ApiResponse register(SignUpRequest request) {
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
        Authentication authentication = authenticationManager.authenticate(
                new UsernamePasswordAuthenticationToken(
                        request.getUsername(),
                        request.getPassword()
                )
        );

        //String token = jwtService.generateToken(authentication);

        return success("User successfully signed in", authentication);
    }
}

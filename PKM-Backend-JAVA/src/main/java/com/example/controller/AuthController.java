package com.example.controller;

import com.example.dto.SignUpRequest;
import io.swagger.v3.oas.annotations.Operation;
import org.springdoc.core.annotations.ParameterObject;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.example.response.ApiResponse;
import com.example.service.AuthService;

@RestController
@RequestMapping("/api/auth")
public class AuthController {
    private final AuthService authService;

    public AuthController(AuthService authService) {
        this.authService = authService;
    }

    @Operation(summary = "Register new user")
    @PostMapping(value = "/register")
    public ResponseEntity<ApiResponse<Object>> register(@RequestBody SignUpRequest request){
        return ResponseEntity.ok(authService.register(request));
    }
}

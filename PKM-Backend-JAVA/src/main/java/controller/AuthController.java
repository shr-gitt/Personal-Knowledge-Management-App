package controller;

import dto.SignUpRequest;
import io.swagger.v3.oas.annotations.Operation;
import org.springdoc.core.annotations.ParameterObject;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import response.ApiResponse;
import service.AuthService;

@RestController
@RequestMapping("/api/auth")
public class AuthController {
    private final AuthService authService;

    public AuthController(AuthService authService) {
        this.authService = authService;
    }

    @Operation(summary = "Register new user")
    @PostMapping(value = "/register", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<ApiResponse<Object>> register(@ParameterObject SignUpRequest request){

        return ResponseEntity.ok(authService.register(request));
    }
}

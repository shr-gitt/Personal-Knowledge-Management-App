package com.example.service;

import com.example.exception.BusinessException;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import com.example.repository.UserRepository;

@Service
public class CustomUserDetailsService implements UserDetailsService {
    private final UserRepository userRepository;

    public CustomUserDetailsService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    public UserDetails loadUserByUsername(String username) {
        com.example.model.User user = userRepository.findByUsername(username)
                .orElseThrow(() -> new BusinessException("User not found"));

        return User.withUsername(user.getUsername())
                .password(user.getPassword())
                .build();
    }
}

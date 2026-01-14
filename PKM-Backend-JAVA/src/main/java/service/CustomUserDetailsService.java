package service;

import exception.BusinessException;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.stereotype.Service;
import repository.UserRepository;

@Service
public class CustomUserDetailsService implements UserDetailsService {
    private final UserRepository userRepository;

    public CustomUserDetailsService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    public UserDetails loadUserByUsername(String username) {
        model.User user = userRepository.findByUsername(username)
                .orElseThrow(() -> new BusinessException("User not found"));

        return User.withUsername(user.getEmail())
                .password(user.getPassword())
                .build();
    }
}

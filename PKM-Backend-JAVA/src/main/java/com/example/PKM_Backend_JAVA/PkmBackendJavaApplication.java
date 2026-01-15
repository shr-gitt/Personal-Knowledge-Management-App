package com.example.PKM_Backend_JAVA;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.data.mongodb.repository.config.EnableMongoRepositories;

// Scan the parent `com.example` package so controllers in `com.example.controller` are discovered
@SpringBootApplication(scanBasePackages = "com.example")
@EnableMongoRepositories(basePackages = "com.example.repository")
public class PkmBackendJavaApplication {

	public static void main(String[] args) {
		SpringApplication.run(PkmBackendJavaApplication.class, args);
	}

}

package com.example.service;

import com.example.exception.BusinessException;
import com.example.exception.ValidationException;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.util.UUID;

@Service
public class UploadImageService {
    public String uploadImage(MultipartFile file) {
        String[] validExtensions = new String[]{"png", "jpg", "jpeg", "gif", "bmp"};

        if (file == null || file.isEmpty()) {
            throw new ValidationException("Image file is required");
        }

        if (!file.getContentType().startsWith("image/")) {
            throw new ValidationException("Invalid image content type");
        }

        if (file.getSize() > 5 * 1024 * 1024) {
            throw new ValidationException("Image must be under 5MB");
        }

        String originalFilename = file.getOriginalFilename();

        if (originalFilename == null || !originalFilename.contains(".")) {
            throw new ValidationException("Invalid image file name");
        }

        String suffix = originalFilename.substring(originalFilename.lastIndexOf(".")+1).toLowerCase();

        boolean isValid = false;

        for(String validExtension:validExtensions){
            if(validExtension.equals(suffix)){
                isValid = true;
                break;
            }
        }

        if(!isValid){
            throw new ValidationException("Invalid image extension");
        }

        String fileName = UUID.randomUUID().toString()+"."+suffix;
    try{
        Path directory = Paths.get(System.getProperty("user.dir"), "uploads", "images");

        if(!Files.exists(directory))
            Files.createDirectories(directory);

        Path fullPath = directory.resolve(fileName);
        try(InputStream inputStream = file.getInputStream()){
            Files.copy(inputStream,fullPath, StandardCopyOption.REPLACE_EXISTING);
        }

        return "/uploads/images/" + fileName;
    }
    catch (Exception ex) {
        throw new BusinessException("Failed to upload image");
    }
    }
}

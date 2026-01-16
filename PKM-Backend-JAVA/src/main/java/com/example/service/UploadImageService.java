package com.example.service;

import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.util.List;
import java.util.UUID;

@Service
public class UploadImageService {
    public String uploadImage(MultipartFile file) throws Exception{
        String[] validExtensions = new String[]{"png", "jpg", "jpeg", "gif", "bmp"};

        String originalFilename = file.getOriginalFilename();

        if(originalFilename==null||originalFilename.length()==0){
            throw new Exception("Invalid path");
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
            throw new Exception("Invalid extension");
        }

        String fileName = UUID.randomUUID().toString()+"."+suffix;

        Path directory = Paths.get(System.getProperty("user.dir"), "uploads", "images");

        if(!Files.exists(directory))
            Files.createDirectories(directory);

        Path fullPath = directory.resolve(fileName);
        try(InputStream inputStream = file.getInputStream()){
            Files.copy(inputStream,fullPath, StandardCopyOption.REPLACE_EXISTING);
        }

        return "/uploads/images/" + fileName;
    }
}

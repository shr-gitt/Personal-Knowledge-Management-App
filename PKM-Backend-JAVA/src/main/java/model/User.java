package model;

import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import java.io.File;
import java.util.List;

@Document(collection = "applicationUsers")
public class User {

    @Id
    private String id;
    private String username;
    private String name;
    private String email;
    private boolean emailConfirmed;
    private String password;
    private double phoneNumber;
    private boolean TwoFactorEnabled;
    private String image;

    protected User(){

    }

    public User(String username, String name, String email, String password, double phoneNumber, String image) {
        this.username = username;
        this.name = name;
        this.email = email;
        this.password = password;
        this.phoneNumber = phoneNumber;
        this.image = image;
    }

    public String getId() {
        return id;
    }

    public String getUsername() {
        return username;
    }

    public String getName() {
        return name;
    }

    public String getEmail() {
        return email;
    }

    public String getPassword() {
        return password;
    }

    public double getPhoneNumber() {
        return phoneNumber;
    }

    public String getImage() {
        return image;
    }

    public void changePassword(String encodedPassword) {
        if(encodedPassword == null || encodedPassword.isEmpty()){
            throw  new IllegalArgumentException("Password cannot be empty");
        }
        this.password = encodedPassword;
    }
}

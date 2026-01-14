package model;

import lombok.Getter;
import lombok.Setter;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import java.io.File;
import java.util.List;

@Document(collection = "applicationUsers")
@Getter
@Setter
public class User {

    @Id
    private String id;
    private String username;
    private String name;
    private String email;
    private boolean emailConfirmed;
    private String password;
    private String phoneNumber;
    private boolean TwoFactorEnabled;
    private String image;

    public User(){

    }

    public User(String username, String name, String email, String password, String phoneNumber, String image) {
        this.username = username;
        this.name = name;
        this.email = email;
        this.password = password;
        this.phoneNumber = phoneNumber;
        this.image = image;
    }

    public void changePassword(String encodedPassword) {
        if(encodedPassword == null || encodedPassword.isEmpty()){
            throw  new IllegalArgumentException("Password cannot be empty");
        }
        this.password = encodedPassword;
    }
}

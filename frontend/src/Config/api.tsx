const baseUrl = 'https://localhost:7032/api'
const baseAuth = `${baseUrl}/Auth`
const baseNotes = `${baseUrl}/Notes`

export const apis = {
    auth: {
        getAllUsers: `${baseAuth}/GetAllUsers`,
        getUserByUsername: `${baseAuth}/GetUserByUsername`,
        register: `${baseAuth}/Register`,
        login: `${baseAuth}/LogIn`,
        logout: `${baseAuth}/LogOut`,
        updateProfile: `${baseAuth}/UpdateUserProfile`,
        deleteProfile: `${baseAuth}/DeleteUserProfile`,
        forgotPassword: `${baseAuth}/ForgotPassword`,
        resetPassword: `${baseAuth}/ResetPassword`,
        changePassword: `${baseAuth}/ChangePassword`,
    },

    notes: {
        getAll: `${baseNotes}/GetAllNotes`,
        get: `${baseNotes}/GetNote`,
        create: `${baseNotes}/CreateNote`,
        delete: `${baseNotes}/DeleteNote`,
        update: `${baseNotes}/UpdateNote`,
    },
};
namespace Application.Infraestructure.Data.Repositories.Scripts;

internal static class SqlUsuario
{
    internal static string InsertUsuario = @"
        INSERT INTO aspnet_users (
            id, 
            user_name, 
            normalized_user_name,
            email, 
            normalized_email, 
            email_confirmed,
            password_hash, 
            security_stamp, 
            concurrency_stamp,
            phone_number, 
            phone_number_confirmed,
            two_factor_enabled, 
            lockout_end,
            lockout_enabled, 
            access_failed_count,
            criado_em
        )
        VALUES (
            @Id, 
            @User_Name, 
            @Normalized_User_Name,
            @Email, 
            @Normalized_Email, 
            @Email_Confirmed,
            @Password_Hash, 
            @Security_Stamp, 
            @Concurrency_Stamp,
            @Phone_Number, 
            @Phone_Number_Confirmed,
            @Two_Factor_Enabled, 
            @Lockout_End,
            @Lockout_Enabled, 
            @Access_Failed_Count,
            @Criado_Em
        ) RETURNING id;
    ";

    internal static string SelectUsuario = @"
        SELECT
            id, 
            user_name, 
            normalized_user_name,
            email, 
            normalized_email, 
            email_confirmed,
            password_hash, 
            security_stamp, 
            concurrency_stamp,
            phone_number, 
            phone_number_confirmed,
            two_factor_enabled, 
            lockout_end,
            lockout_enabled, 
            access_failed_count,
            criado_em
        FROM aspnet_users
        WHERE 1 = 1
    ";
}

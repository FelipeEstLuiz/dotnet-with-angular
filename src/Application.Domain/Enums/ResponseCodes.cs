﻿namespace Application.Domain.Enums;

public enum ResponseCodes
{
    NONE,
    USER_NOT_FOUND,
    EMAIL_NOT_FOUND,
    BAD_REQUEST,
    INTERNAL_SERVER_ERROR,
    INVALID_MODEL,
    UNAUTHORIZED,
    INCORRECT_PASSWORD,
    ALREADY_EXISTS,
    PENDING_ALTER_PASSWORD,
    WRONG_PASSWORD,
    ACCOUNT_PENDING_ACTIVATION,
    TOKEN_NOT_FOUND,
    EMAIL_ALREADY_EXISTS,
    CATEGORY_NOT_FOUND,
    USER_NOT_A_BUSINESS,
    CATEGORY_ALREADY_EXISTS,
    NOT_A_IMAGE,
    INVALID_LENGTH,
    USER_NOT_HAVE_PERMISSION
}
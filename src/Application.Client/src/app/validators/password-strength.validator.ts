export function getPasswordStrength(password: string): 'Weak' | 'Average' | 'Strong' {
    const hasLower = /[a-z]/.test(password);
    const hasUpper = /[A-Z]/.test(password);
    const hasNumber = /[0-9]/.test(password);
    const hasSymbol = /[\W_]/.test(password);
  
    const strengthCount = [hasLower, hasUpper, hasNumber, hasSymbol].filter(Boolean).length;
  
    if (password.length < 6 || strengthCount < 2) return 'Weak';
    if (password.length >= 6 && strengthCount === 3) return 'Average';
    if (password.length >= 8 && strengthCount === 4) return 'Strong';
  
    return 'Weak';
  }
  
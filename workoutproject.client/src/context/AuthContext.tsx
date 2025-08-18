import React, { createContext, useContext, useState } from 'react';
import type { Tokens } from '../shared/api/auth';
import { getStoredTokens, login as apiLogin, clearTokens } from '../shared/api/auth';

interface AuthContextValue {
  tokens: Tokens | null;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [tokens, setTokens] = useState<Tokens | null>(() => getStoredTokens());

  const login = async (username: string, password: string) => {
    const newTokens = await apiLogin(username, password);
    setTokens(newTokens);
  };

  const logout = () => {
    clearTokens();
    setTokens(null);
  };

  return (
    <AuthContext.Provider value={{ tokens, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
}

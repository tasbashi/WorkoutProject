import React from 'react';
import { useAuth } from './context/AuthContext';
import Login from './components/Login';

const App: React.FC = () => {
  const { tokens, logout } = useAuth();

  if (!tokens) {
    return <Login />;
  }

  return (
    <div style={{ padding: '1rem' }}>
      <h1>Authenticated</h1>
      <p>Access token: {tokens.accessToken}</p>
      <button onClick={logout}>Logout</button>
    </div>
  );
};

export default App;

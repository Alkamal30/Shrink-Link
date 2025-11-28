import React, { useState } from 'react';
import './TunnelLogin.css';

const TunnelLogin: React.FC = () => {
  const [isLogin, setIsLogin] = useState(false);

  return (
    <div className="scene-container">
      {/* 
        Контейнер мира. 
        state-home: Z = 0
        state-login: Z = 5000
      */}
      <div className={`world ${isLogin ? 'state-login' : 'state-home'}`}>
      {/* <div className={`world state-home`}> */}
        
        {/* --- Landing Page (Z=0) --- */}
        <div className="section landing-page">
          <div className="landing-content">
            <h1>Shrink Link</h1>
            <p>Shrink link — in a blink.</p>
            <button className="btn-primary" onClick={() => setIsLogin(true)}>
              START
            </button>
          </div>
        </div>

        {/* --- 3D Corridor --- */}
        <div className="corridor">
          <div className="wall wall-left" />
          <div className="wall wall-right" />
          <div className="wall wall-top" />
          <div className="wall wall-bottom" />
        </div>

        {/* --- Login Page (Z=-5000) --- */}
        <div className="section login-page">
          <div className="login-card">
            <h2 style={{ margin: '0 0 30px 0', fontWeight: 400 }}>Identify</h2>
            
            <input type="text" placeholder="Username" className="input-field" />
            <input type="password" placeholder="Password" className="input-field" />
            
            <button 
              className="btn-primary" 
              style={{ width: '100%', marginTop: '20px' }}
            >
              LOGIN
            </button>
            
            <div 
              style={{ marginTop: '25px', color: '#555', cursor: 'pointer', fontSize: '0.9rem' }}
              onClick={() => setIsLogin(false)}
            >
              Cancel & Return
            </div>
          </div>
        </div>

      </div>
    </div>
  );
};

export default TunnelLogin;
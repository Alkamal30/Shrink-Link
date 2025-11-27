import React, { useEffect, useRef, useState } from 'react';
import './Cursor.css';

const Cursor: React.FC = () => {
  const cursorRef = useRef<HTMLDivElement>(null);
  
  const mousePosition = useRef({ x: 0, y: 0 });
  const cursorPosition = useRef({ x: 0, y: 0 });
  
  const [isClicked, setIsClicked] = useState(false);

  useEffect(() => {
    const onMouseMove = (event: MouseEvent) => {
      mousePosition.current = { x: event.clientX, y: event.clientY };
    };

    const onMouseDown = () => setIsClicked(true);
    const onMouseUp = () => setIsClicked(false);

    window.addEventListener('mousemove', onMouseMove);
    window.addEventListener('mousedown', onMouseDown);
    window.addEventListener('mouseup', onMouseUp);

    let animationFrameId: number;
    
    const loop = () => {
      const cursor = cursorRef.current;
      
      if (cursor) {
        const speed = 0.20;
        
        cursorPosition.current.x += (mousePosition.current.x - cursorPosition.current.x) * speed;
        cursorPosition.current.y += (mousePosition.current.y - cursorPosition.current.y) * speed;

        cursor.style.transform = `translate3d(${cursorPosition.current.x}px, ${cursorPosition.current.y}px, 0) scale(${isClicked ? 0.8 : 1})`;
      }

      animationFrameId = requestAnimationFrame(loop);
    };

    loop();

    return () => {
      window.removeEventListener('mousemove', onMouseMove);
      window.removeEventListener('mousedown', onMouseDown);
      window.removeEventListener('mouseup', onMouseUp);
      cancelAnimationFrame(animationFrameId);
    };
  }, [isClicked]);

  return (
    <div 
      ref={cursorRef} 
      className={`custom-cursor ${isClicked ? 'clicked' : ''}`} 
    />
  );
};

export default Cursor;
import React, { useState, useRef } from 'react'
import Quiz from './Components/Quiz/Quiz'
import './App.css'


const App = () => {
  const [nameStatus, setNameStatus] = useState(false);
  const [username, setUsername] = useState('');

  const handleNameSubmit = (e) => {
    e.preventDefault();
    // You can perform any validation or processing here before setting the name status
    setNameStatus(true);
  };

  const handleNameChange = (e) => {
    setUsername(e.target.value);
  };

  const submit = () => {
    const questionNumber = 0;
    const marks =0;
    const level = 0;
    const coins = 0;

    const player = {username, questionNumber, marks, level, coins};
    fetch("http://localhost:8080/player/add", {
            method:"POST",
            headers:{"Content-Type":"application/json"},
            body:JSON.stringify(player)
        }).then(()=>{
            console.log("player updated",JSON.stringify(player));
        })
  }
  
  return (
    <div className='app-container'>
      {nameStatus?<></>:<>
      
        <form className="form" onSubmit={handleNameSubmit}>
        <label htmlFor="username">
        <h1 className='h1'>WELCOME TO ENERGY ELIXIRE  QUESTIONNAIRE !</h1>

        <li className='li'>This quiz consists of multiple-choice questions related to energy conservation and efficiency. </li>
        <li className='li'>You'll be rewarded in the game based on your answers! 
        The more correct answers you provide, the greater your rewards will be. </li>
        <li className='li'>give it your best shot and enjoy reaping the benefits of your energy-saving expertise!</li>
        
        <h3 className='h2'>Good luck, and have fun !</h3>
          <p className='text'>Please Enter your username:</p>
        </label>
        <input type="text" id="username" value={username} onChange={handleNameChange} required
        className='input'/>
        <p><button type="submit" className='submit' onClick={submit}>Submit</button></p>
        
        </form>
      </>}

      {nameStatus?<>
        <Quiz username={username}/>
      </>:<></>}
      

    </div>
  )
}

export default App;

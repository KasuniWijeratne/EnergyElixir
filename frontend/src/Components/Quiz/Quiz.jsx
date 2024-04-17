import React, { useState, useRef } from 'react'
import './Quiz.css'
import { data } from '../../assets/data';

const Quiz = (name) => {
    console.log(name.username);
    const username = name.username;
    let [index, setIndex] = useState(0);
    let [question, setQuestion] = useState(data[index]);
    let [lock, setLock] = useState(false);
    let [marks,setMarks] = useState(0);
    let [result, setResult] = useState(false);
    let [feedback, setFeedback] = useState(false);
    let [answer, setAnswer] = useState('');
    let [selectedNumber, setSelectedNumber] =useState(0);
    let [comment, setComment] = useState('');


    let Option1 = useRef(null);
    let Option2 = useRef(null);
    let Option3 = useRef(null);
    let Option4 = useRef(null);

    let option_array = [Option1, Option2, Option3, Option4];

    const clickAns = (e,ans) => {
        option_array.map((option)=>{
            option.current.classList.remove("highlight");
            option.current.classList.remove("highlight");
            return null;
        })
        setSelectedNumber(ans);
        setLock(true);
        e.target.classList.add("highlight");
        if (question.ans === ans){
            setAnswer('Correct !!!');
        }
        else{
            setAnswer('your answer is wrong')
        }
    }
    const generateFeedback = () => {
        if (selectedNumber===1){
            setComment(question.option1[1]);
        }
        else if (selectedNumber===2){
            setComment(question.option2[1]);
        }
        else if (selectedNumber===3){
            setComment(question.option3[1]);
        }
        else {
            setComment(question.option4[1]);
        }
    }

    const checkAns = () => {
        if(lock === true){
            if (question.ans === selectedNumber){
                setMarks(prev=> prev+1);
                setLock(false);
            }
            else{
                setLock(false);
            }
            setFeedback(true);
            generateFeedback();
            const questionNumber = index+1;
            const currentQustion =  {username, questionNumber};
            fetch("http://localhost:8080/player/updateQuestionNumber", {
            method:"POST",
            headers:{"Content-Type":"application/json"},
            body:JSON.stringify(currentQustion)
        }).then(()=>{
            console.log("question number updated",JSON.stringify(currentQustion));
        })
        }
    }
    const next = ()=>{

        setFeedback(false);
        if(index===data.length-1){
            setResult(true);
            return 0;
        }
        setIndex(++index);
        setQuestion(data[index]);
        setLock(false);
        option_array.map((option)=>{
            try {
                option.current.classList.remove("wrong");
                option.current.classList.remove("correct");
            } catch (error) {
                console.log();
                }
            return null;
        })}

    const updateDatabase = (e)=>{
        e.preventDefault();
        const player = {username, marks};
        fetch("http://localhost:8080/player/updateMarks", {
            method:"POST",
            headers:{"Content-Type":"application/json"},
            body:JSON.stringify(player)
        }).then(()=>{
            console.log("player updated",JSON.stringify(player));
        }).then(console.log("redirecting to game"))
        
    }

  return (
    <div className='container'>
        <h1>Energy Elixire</h1>
        <hr />
        {result?<></>:<>
            {feedback?<></>:<>
                <h2>{index+1}. {question.question}</h2>
                <ul>
                    <li ref={Option1} onClick={(e)=>{clickAns(e,1)}}>{question.option1[0]}</li>
                    <li ref={Option2} onClick={(e)=>{clickAns(e,2)}}>{question.option2[0]}</li>
                    <li ref={Option3} onClick={(e)=>{clickAns(e,3)}}>{question.option3[0]}</li>
                    <li ref={Option4} onClick={(e)=>{clickAns(e,4)}}>{question.option4[0]}</li>
                </ul>
                <button onClick={checkAns}>Check</button> 

                <div className="index">{index+1} of {data.length} questions</div>
            </>}

            {feedback?<>
                <h3>{answer}</h3>
                <h4>{comment}</h4>
                <h4>{question.GeneralFeedback}</h4>
                
                <button onClick={next}>Next</button>
            </>:<></>}


        </>}
        {result?<>
            <h2>You scored {marks} out of {data.length}</h2>
            <button onClick={updateDatabase}>Go back to game</button>
            </>:<></>}
        
    </div>
  )
}

export default Quiz
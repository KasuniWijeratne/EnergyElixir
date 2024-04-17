package com.energyelixire.energySavingGame.controller;


import com.energyelixire.energySavingGame.model.Player;
import com.energyelixire.energySavingGame.service.PlayerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.NoSuchElementException;

@RestController
@RequestMapping("/player")

public class PlayerController {
    @Autowired
    private PlayerService playerService;

    @PostMapping("/add")
    public String add(@RequestBody Player player) {
        playerService.savePlayer(player);
        return "new player added.";
    }

    @GetMapping("/getAll")
    public List<Player> getAllPlayers() {
        return playerService.getAllPlayers();
    }

    @PostMapping("/updateMarks")
    public ResponseEntity<Player> updateMarks(@RequestBody Player player) {
        try{
            playerService.updateMarks(player);
            return new ResponseEntity<>(player, HttpStatus.OK);
        }
        catch(NoSuchElementException e){
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }

    @PostMapping("/updateQuestionNumber")
    public ResponseEntity<Player> updateQuestionNumber(@RequestBody Player player) {
        try{
            playerService.updateQuestionNumber(player);
            return new ResponseEntity<>(player, HttpStatus.OK);
        }
        catch(NoSuchElementException e){
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }

    @PostMapping("/updateLevel")
    public ResponseEntity<Player> updateLevel(@RequestBody Player player) {
        try{
            playerService.updateLevel(player);
            return new ResponseEntity<>(player, HttpStatus.OK);
        }
        catch(NoSuchElementException e){
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }

    @PostMapping("/updateCoins")
    public ResponseEntity<Player> updateCoins(@RequestBody Player player) {
        try{
            playerService.updateCoins(player);
            return new ResponseEntity<>(player, HttpStatus.OK);
        }
        catch(NoSuchElementException e){
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }

    @PostMapping("/delete")
    public ResponseEntity<Player> deletePlayer(@RequestBody Player player) {
        try{
            playerService.deletePlayer(player);
            return new ResponseEntity<>(player, HttpStatus.OK);
        }
        catch(NoSuchElementException e){
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }



}


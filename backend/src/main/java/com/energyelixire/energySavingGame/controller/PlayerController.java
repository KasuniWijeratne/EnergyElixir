package com.energyelixire.energySavingGame.controller;


import com.energyelixire.energySavingGame.model.Player;
import com.energyelixire.energySavingGame.service.PlayerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

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
}


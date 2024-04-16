package com.energyelixire.energySavingGame.service;

import com.energyelixire.energySavingGame.model.Player;

import java.util.List;

public interface PlayerService {
    public Player savePlayer(Player player);
    public List<Player> getAllPlayers();
}

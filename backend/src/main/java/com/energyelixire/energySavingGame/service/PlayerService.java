package com.energyelixire.energySavingGame.service;

import com.energyelixire.energySavingGame.model.Player;

import java.util.List;

public interface PlayerService {
    public Player savePlayer(Player player);
    public List<Player> getAllPlayers();
    public void updateMarks(Player player);
    public void updateQuestionNumber(Player player);
    public void updateLevel(Player player);
    public void updateCoins(Player player);
    public void deletePlayer(Player player);
    public Player getPlayerByUsername(String username);
}

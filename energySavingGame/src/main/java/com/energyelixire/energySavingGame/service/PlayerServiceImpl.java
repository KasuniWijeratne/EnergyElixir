package com.energyelixire.energySavingGame.service;

import com.energyelixire.energySavingGame.model.Player;
import com.energyelixire.energySavingGame.repository.PlayerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.NoSuchElementException;
import java.util.Optional;

@Service
public class PlayerServiceImpl implements PlayerService {

    @Autowired
    private PlayerRepository playerRepository;
    @Override
    public Player savePlayer(Player player) {
        return playerRepository.save(player);
    }

    @Override
    public List<Player> getAllPlayers() {
        return playerRepository.findAll();
    }

    @Override
    public void updateMarks(Player player){
        String nic = player.getNic();
        Player playerNew = playerRepository.findByNic(nic);
        playerNew.setMarks(player.getMarks());
        playerRepository.save(playerNew);
    }

    @Override
    public void updateQuestionNumber(Player player){
        String nic = player.getNic();
        Player playerNew = playerRepository.findByNic(nic);
        playerNew.setQuestionNumber(player.getQuestionNumber());
        playerRepository.save(playerNew);
    }

    @Override
    public void updateLevel(Player player) {
        String nic = player.getNic();
        Player playerNew = playerRepository.findByNic(nic);
        playerNew.setLevel(player.getLevel());
        playerRepository.save(playerNew);
    }

    @Override
    public void updateCoins(Player player) {
        String nic = player.getNic();
        Player playerNew = playerRepository.findByNic(nic);
        playerNew.setCoins(player.getCoins());
        playerRepository.save(playerNew);
    }

    @Override
    public void deletePlayer(Player player) {
        String nic = player.getNic();
        Player playerNew = playerRepository.findByNic(nic);
        playerRepository.delete(playerNew);
    }
}



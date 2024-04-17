package com.energyelixire.energySavingGame.repository;

import com.energyelixire.energySavingGame.model.Player;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface PlayerRepository extends JpaRepository<Player, Integer> {
    public Player findByUsername(String username);
}

package com.energyelixir.demo;

import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.reactive.function.client.WebClient;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;

import lombok.extern.log4j.Log4j2;
import reactor.core.publisher.Mono;

import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.PathVariable;


@Log4j2
@RestController
public class ApiHandler {

    String baseUrl = "http://20.15.114.131:8080";
    WebClient.Builder builder = WebClient.builder();
    WebClient webClient = builder.baseUrl(baseUrl).build();
    String apiKey = "NjVjNjA0MGY0Njc3MGQ1YzY2MTcyMmNkOjY1YzYwNDBmNDY3NzBkNWM2NjE3MjJjMw";
    ObjectMapper objectMapper = new ObjectMapper();
    JsonNode apikey = objectMapper.createObjectNode().put("apiKey", apiKey);
    String token;

    ApiHandler() {

    }

    @GetMapping("/login")
    public Mono<ResponseEntity<String>> login() {
        return webClient.post().uri("/api/login")
                .contentType(MediaType.APPLICATION_JSON)
                .bodyValue(apikey)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    token = response.get("token").asText();
                    return ResponseEntity.ok().body("ok");
                }).onErrorResume(e -> {
                    log.error("Error occured during logging in", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during logging in"));
                });
    }

    @GetMapping("/user/profile/view")
    public Mono<ResponseEntity<String>> profileView() {
        return webClient.get()
                .uri("/api/user/profile/view")
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile view"));
                });
    }

    @PutMapping("/user/profile/update")
    public Mono<ResponseEntity<String>> profileUpdate(@RequestBody JsonNode requestBody) {
        return webClient.put()
                .uri("/api/user/profile/update")
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .contentType(MediaType.APPLICATION_JSON)
                .bodyValue(requestBody)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile view"));
                });
    }
    

    @GetMapping("/user/profile/list")
    public Mono<ResponseEntity<String>> profileList() {
        return webClient.get()
                .uri("/api/user/profile/list")
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile view"));
                });
    }

    @GetMapping("/power-consumption/yearly/view")
    public Mono<ResponseEntity<String>> yearlyConsumption(@RequestParam("year") final String yearly) {
        return webClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/api/power-consumption/yearly/view")
                        .queryParam("year", yearly)
                        .build())
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile list"));
                });
    }

    @GetMapping("/power-consumption/month/view")
    public Mono<ResponseEntity<String>> ConsumptionInMonth(@RequestParam("year") final String yearly , @RequestParam("month") final String monthly) {
        return webClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/api/power-consumption/month/view")
                        .queryParam("year", yearly)
                        .queryParam("month" , monthly)
                        .build())
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile list"));
                });
    }

    @GetMapping("/power-consumption/month/daily/view")
    public Mono<ResponseEntity<String>> dailyConsumptionInMonth(@RequestParam("year") final String yearly , @RequestParam("month") final String monthly) {
        return webClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/api/power-consumption/month/daily/view")
                        .queryParam("year", yearly)
                        .queryParam("month" , monthly)
                        .build())
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile list"));
                });
    }

    @GetMapping("/power-consumption/current-month/view")
    public Mono<ResponseEntity<String>> ConsumptionInCurrentMonth() {
        return webClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/api/power-consumption/current-month/view")
                        .build())
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile list"));
                });
    }

    @GetMapping("/power-consumption/current-month/daily/view")
    public Mono<ResponseEntity<String>> dailyConsumptionInCurrentMonth() {
        return webClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/api/power-consumption/current-month/daily/view")
                        .build())
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile list"));
                });
    }

    @GetMapping("/power-consumption/all/view")
    public Mono<ResponseEntity<String>> allConsumption() {
        return webClient.get()
                .uri(uriBuilder -> uriBuilder
                        .path("/api/power-consumption/all/view")
                        .build())
                .header(HttpHeaders.AUTHORIZATION, "Bearer " + token)
                .retrieve()
                .bodyToMono(JsonNode.class)
                .map(response -> {
                    return ResponseEntity.ok().body(response.toString()); // reply to the button press or something send
                                                                          // it accordingly
                }).onErrorResume(e -> {
                    log.error("Error", e);
                    return Mono.just(ResponseEntity.status(500).body("Error occured during profile list"));
                });
    }
}

package com.cs3250;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;

/**
 * Jordan Ross
 * CS 3250
 * Assignment 2
 * This class is used for searching a test file for words given a search criteria and path to the file
 */
public class Search {
    private static String[] filters;
    private static boolean noMatchesFound = true;

    /**
     * The starting point of this simple search engine
     * @param args args[0] is the path to the text file containing words, args[1] is the search criteria
     */
    public static void main(String args[]) {
        //a path to a text file must be specified
        if (args.length == 0) {
            System.out.println("incorrect file, please try again");
            return;
        }

        //if args.length == 2 filters were passed in
        if (args.length == 2) {
            filters = args[1].split("\\s+");
            if(filters.length < 2) {
                exit();
            }
        } else if (args.length > 2) {
            System.out.println("too many parameters, max allowed is 2");
            return;
        }

        File file = new File(args[0]);
        if (!file.exists() || file.isDirectory()) {
            System.out.println("incorrect file, please try again");
            return;
        }

        try (BufferedReader br = new BufferedReader(new FileReader(args[0]))) {
            String currentWord;

            int line = 1;
            while ((currentWord = br.readLine()) != null) {
                currentWord = currentWord.trim().toLowerCase();
                //if args.length == 1 filtersToSearchBy will be null just list all the words
                if (filters != null) {
                    if (filters[filters.length - 1].equals("orMatch")) {
                        if (wordMatchesOrSearch(currentWord)) {
                            noMatchesFound = false;
                            System.out.println(line + " " + currentWord);
                        }
                    } else {
                        if (wordMatchesAndSearch(currentWord)) {
                            noMatchesFound = false;
                            System.out.println(line + " " + currentWord);
                        }
                    }

                } else {
                    noMatchesFound = false;
                    System.out.println(line + " " + currentWord);
                }
                line++;
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        if (noMatchesFound) {
            System.out.println("no such match");
        }
    }

    /***
     * Checks if the current word matches any of the filters
     * @param currentWord current word read in from the text file
     * @return if the word matched any of the filters
     */
    private static boolean wordMatchesOrSearch(String currentWord) {
        boolean lastIndexHadFilter = false;

        for (int i = 0; i < filters.length - 1; i++) {
            if (lastIndexHadFilter) {
                lastIndexHadFilter = false;
                continue;
            }
            String filter = filters[i];
            String character = filters[i + 1].toLowerCase();

            exitIfStringIsNotALetterOrANumber(character);
            if (filter.equals("^")) {
                lastIndexHadFilter = true;
                if (wordStartsWithCorrectLetter(currentWord, character)) {
                    return true;
                }
            } else if (filter.equals("$")) {
                lastIndexHadFilter = true;
                if (wordEndsWithCorrectLetter(currentWord, character)) {
                    return true;
                }
            } else if (filter.equals("*")) {
                lastIndexHadFilter = true;
                if (wordContainsLetter(currentWord, character)) {
                    return true;
                }
            } else if (isInteger(filter)) {
                lastIndexHadFilter = true;
                if (wordContainsCharacterAtPosition(currentWord, filter, character)) {
                    return true;
                }
            } else {
                exit();
            }
        }

        return false;
    }

    /**
     * Checks if the current word matches all of the filters
     * @param currentWord current word read in from the text file
     * @return if the word matched all of the filters
     */
    private static boolean wordMatchesAndSearch(String currentWord) {
        boolean lastIndexHadFilter = false;

        for (int i = 0; i < filters.length - 1; i++) {
            if (lastIndexHadFilter) {
                lastIndexHadFilter = false;
                continue;
            }
            String filter = filters[i];
            String character = filters[i + 1].toLowerCase();

            exitIfStringIsNotALetterOrANumber(character);
            if (filter.equals("^")) {
                lastIndexHadFilter = true;
                if (!wordStartsWithCorrectLetter(currentWord, character)) {
                    return false;
                }
            } else if (filter.equals("$")) {
                lastIndexHadFilter = true;
                if (!wordEndsWithCorrectLetter(currentWord, character)) {
                    return false;
                }
            } else if (filter.equals("*")) {
                lastIndexHadFilter = true;
                if (!wordContainsLetter(currentWord, character)) {
                    return false;
                }
            } else if (isInteger(filter)) {
                lastIndexHadFilter = true;
                if (!wordContainsCharacterAtPosition(currentWord, filter, character)) {
                    return false;
                }
            } else {
                exit();
            }
        }

        return true;
    }

    /**
     * Checks if the given word contains a character at a position
     * @param currentWord current word we are checking
     * @param position position to check
     * @param character character expected at the position
     * @return if the word contains the character at the position
     */
    private static boolean wordContainsCharacterAtPosition(String currentWord, String position, String character) {
        int pos = Integer.parseInt(position) - 1;
        if (currentWord.length() > pos) {
            String letter = String.valueOf(currentWord.toCharArray()[pos]);
            return letter.equals(character);
        }
        return false;
    }

    /**
     * Checks if a word contains a letter
     * @param currentWord word to check
     * @param letter letter we are checking for
     * @return if the word contains the letter
     */
    private static boolean wordContainsLetter(String currentWord, String letter) {
        return currentWord.contains(letter);
    }

    /**
     * Checks if a word ends with a letter
     * @param currentWord word to check
     * @param letter expected last letter of the word
     * @return if the word ends with the letter
     */
    private static boolean wordEndsWithCorrectLetter(String currentWord, String letter) {
        return currentWord.substring(currentWord.length() - 1).equals(letter);
    }

    /**
     * Checks if a word starts with a letter
     * @param currentWord word to check
     * @param letter expected first letter of the word
     * @return if the word starts with the letter
     */
    private static boolean wordStartsWithCorrectLetter(String currentWord, String letter) {
        return currentWord.substring(0, 1).equals(letter);
    }

    /**
     * Exits the program
     */
    private static void exit() {
        System.out.println("incorrect syntax, please try again");
        System.exit(1);
    }

    /**
     * Checks if a string is a letter or number, if it's not the program will exit
     * @param letter letter/number to check
     */
    private static void exitIfStringIsNotALetterOrANumber(String letter) {
        if (letter.length() == 1 || Character.isLetter(letter.toCharArray()[0]) || isInteger(letter)) {
            return;
        }
        exit();
    }

    /**
     * Checks if a string contains an integer value
     * @param number string value to check
     * @return if the string value contains an int
     */
    private static boolean isInteger(String number) {
        try {
            int value = Integer.parseInt(number);
            if (value == 0) {
                System.out.println("incorrect syntax, please try again");
                System.exit(1);
            }
            return true;
        } catch (NumberFormatException e) {
            return false;
        }
    }
}

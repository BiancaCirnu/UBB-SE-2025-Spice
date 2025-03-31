CREATE OR ALTER PROCEDURE AddGameToCollection
    @collection_id INT,
    @game_id INT
AS
BEGIN
    -- Check if collection exists
    IF NOT EXISTS (SELECT 1 FROM Collections WHERE collection_id = @collection_id)
    BEGIN
        RAISERROR('Collection not found', 16, 1)
        RETURN
    END

    -- Check if game exists
    IF NOT EXISTS (SELECT 1 FROM OwnedGames WHERE game_id = @game_id)
    BEGIN
        RAISERROR('Game not found', 16, 1)
        RETURN
    END

    -- Check if game is already in collection
    IF EXISTS (SELECT 1 FROM OwnedGames_Collection WHERE collection_id = @collection_id AND game_id = @game_id)
    BEGIN
        RAISERROR('Game is already in collection', 16, 1)
        RETURN
    END

    -- Add game to collection
    INSERT INTO OwnedGames_Collection (collection_id, game_id)
    VALUES (@collection_id, @game_id)
END 
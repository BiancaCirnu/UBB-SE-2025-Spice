CREATE OR ALTER PROCEDURE RemoveGameFromCollection
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

    -- Remove game from collection
    DELETE FROM OwnedGames_Collection
    WHERE collection_id = @collection_id AND game_id = @game_id
END
GO 
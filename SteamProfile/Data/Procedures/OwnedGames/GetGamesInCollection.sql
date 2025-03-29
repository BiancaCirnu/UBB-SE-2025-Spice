CREATE OR ALTER PROCEDURE GetGamesInCollection
    @collection_id INT
AS
BEGIN
    -- Check if collection exists
    IF NOT EXISTS (SELECT 1 FROM Collections WHERE collection_id = @collection_id)
    BEGIN
        RAISERROR('Collection not found', 16, 1)
        RETURN
    END

    -- Get all games in collection
    SELECT og.game_id, og.user_id, og.title, og.description, og.cover_picture
    FROM OwnedGames og
    INNER JOIN OwnedGames_Collection ogc ON og.game_id = ogc.game_id
    WHERE ogc.collection_id = @collection_id
    ORDER BY og.title
END
GO 
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

    -- Get the user_id who owns this collection
    DECLARE @user_id INT
    SELECT @user_id = user_id FROM Collections WHERE collection_id = @collection_id

    -- If this is the "All Owned Games" collection (collection_id = 1)
    IF @collection_id = 1
    BEGIN
        -- Return all games owned by the user (without duplicates)
        SELECT DISTINCT og.game_id, og.user_id, og.title, og.description, og.cover_picture
        FROM OwnedGames og
        WHERE og.user_id = @user_id
        ORDER BY og.title
    END
    ELSE
    BEGIN
        -- For other collections, return games from the collection that belong to the user
        SELECT og.game_id, og.user_id, og.title, og.description, og.cover_picture
        FROM OwnedGames og
        INNER JOIN OwnedGames_Collection ogc ON og.game_id = ogc.game_id
        WHERE ogc.collection_id = @collection_id
        AND og.user_id = @user_id
        ORDER BY og.title
    END
END
GO 
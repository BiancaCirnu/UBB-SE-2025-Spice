CREATE OR ALTER PROCEDURE DeleteCollection
    @user_id INT,
    @collection_id INT
AS
BEGIN
    -- Check if collection exists
    IF NOT EXISTS (SELECT 1 FROM Collections WHERE collection_id = @collection_id)
    BEGIN
        RAISERROR('Collection not found', 16, 1)
        RETURN
    END

    -- Delete the collection
    DELETE FROM Collections
    WHERE collection_id = @collection_id AND user_id = @user_id;
END
GO 
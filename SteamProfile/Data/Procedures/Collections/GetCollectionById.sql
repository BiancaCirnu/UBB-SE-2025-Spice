CREATE PROCEDURE GetCollectionById
    @collectionId INT
AS
BEGIN
    SELECT 
        collection_id,
        user_id,
        name,
        cover_picture,
        is_public,
        created_at
    FROM Collections
    WHERE collection_id = @collectionId
END
